import { defineStore } from "pinia";
import type { Transformer } from "@/domain/entities/transformer/Transformer";
import { SignalRTransformerDataSource } from "@/mock/SignalRTransformerDataSource";
import { TransformerApiRepository } from "@/data/repositories/transormer/TransformerApiRepository";
import { alertService } from "@/application/services/AlertService";
import { useCardsStore } from "./card.store";

export const useTransformersStore = defineStore("transformers", {
  state: () => ({
    transformers: [] as Transformer[],
    isLoaded: false,
    dataSource: null as SignalRTransformerDataSource | null,
    repository: new TransformerApiRepository(),
  }),

  actions: {
    async load() {
      if (this.isLoaded) return;

      try {
        const initialData = await this.repository.getAll();
        this.transformers = initialData;
        
        const cardsStore = useCardsStore();
        await cardsStore.load(); // Ensure cards are loaded first

        if (cardsStore.cards.length === 0) {
          cardsStore.addCard({
            id: crypto.randomUUID(),
            title: "Live Assets Monitoring",
            type: "transformer",
            settings: {
              limit: 10,
              sortBy: "name",
              sortDir: "asc",
              health: "All",
              showChart: true,
              refreshInterval: 10,
              selectedTransformerIds: []
            }
          });
        }

        this.isLoaded = true;

        if (!this.dataSource) {
          this.dataSource = new SignalRTransformerDataSource();
          await this.dataSource.subscribe((update: any) => {
            const transformer = this.transformers.find(t => t.id === update.transformerId || t.assetId === update.transformerId);
            
            if (transformer) {
              const newReading = {
                timestamp: new Date(update.timestamp),
                voltage: update.voltageValue
              };

              transformer.voltageReadings.unshift(newReading);
              if (transformer.voltageReadings.length > 10) {
                transformer.voltageReadings.pop();
              }

              alertService.processTransformers(this.transformers);
            }
          });
        }
      } catch (error) {
        console.error("Failed to load initial transformer data:", error);
      }
    },

    configureMonitoring(ids: number[], intervalSeconds: number) {
      // In full-stack mode, this could send a command to backend
      // For now, we ensure the local state is ready
      console.log(`Configuring monitoring for ${ids.length} assets at ${intervalSeconds}s`);
    },

    dispose() {
      this.dataSource?.unsubscribe();
      this.dataSource = null;
      this.isLoaded = false;
    },
  },
});
