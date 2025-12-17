import { defineStore } from "pinia";
import type { Transformer } from "@/domain/entities/transformer/Transformer";
import type { CardModel } from "@/domain/entities/card/CardModel";

export const useCardsStore = defineStore("cards", {
  state: () => ({
    cards: [] as CardModel[],
    loaded: false,

    activeCardId: null as string | null,
    settingsMode: "edit" as "edit" | "confirm-delete",
  }),

  getters: {
    apply: () => (card: CardModel, items: Transformer[]) => {
      let list = [...items];

      if (
        card.settings.selectedTransformerIds &&
        card.settings.selectedTransformerIds.length > 0
      ) {
        list = list.filter((t) =>
          card.settings.selectedTransformerIds!.includes(t.assetId)
        );
      }

      if (card.settings.health !== "All") {
        list = list.filter((t) => t.health === card.settings.health);
      }

      const q = (card.settings.search ?? "").trim().toLowerCase();
      if (q) {
        list = list.filter(
          (t) =>
            t.name.toLowerCase().includes(q) ||
            t.assetId.toString().includes(q) ||
            t.region.toLowerCase().includes(q)
        );
      }

      const rank: Record<Transformer["health"], number> = {
        Critical: 0,
        Poor: 1,
        Fair: 2,
        Good: 3,
        Excellent: 4,
      };

      list.sort((a, b) => {
        let r = 0;
        switch (card.settings.sortBy) {
          case "id":
            r = a.assetId - b.assetId;
            break;
          case "name":
            r = a.name.localeCompare(b.name);
            break;
          case "region":
            r = a.region.localeCompare(b.region);
            break;
          case "health":
            r = rank[a.health] - rank[b.health];
            break;
        }
        return card.settings.sortDir === "asc" ? r : -r;
      });

      return list.slice(0, Math.max(1, card.settings.limit));
    },
  },

  actions: {
    async load() {
      const cached = localStorage.getItem("cards.registry");
      if (cached) {
        try {
          this.cards = JSON.parse(cached).cards ?? [];
          this.loaded = true;
          return;
        } catch {}
      }

      try {
        const res = await fetch("/config/cards.registry.json");
        if (!res.ok) throw new Error();
        const json = await res.json();
        this.cards = Array.isArray(json.cards) ? json.cards : [];
      } catch {
        this.cards = [];
      } finally {
        this.loaded = true;
      }
    },

    save() {
      localStorage.setItem(
        "cards.registry",
        JSON.stringify({ cards: this.cards }, null, 2)
      );
    },

    addCard(card: CardModel) {
      this.cards.push(card);
      this.save();
    },

    openSettings(cardId: string) {
      this.activeCardId = cardId;
      this.settingsMode = "edit";
    },

    closeSettings() {
      this.activeCardId = null;
      this.settingsMode = "edit";
    },

    updateCardSetting(cardId: string, key: string, value: any) {
      const card = this.cards.find((c) => c.id === cardId);
      if (!card) return;

      (card.settings as any)[key] = value;
      this.save();
    },

    requestDelete(cardId: string) {
      this.activeCardId = cardId;
      this.settingsMode = "confirm-delete";
    },

    confirmDelete() {
      if (!this.activeCardId) return;

      this.cards = this.cards.filter((c) => c.id !== this.activeCardId);

      this.activeCardId = null;
      this.settingsMode = "edit";
      this.save();
    },
  },
});
