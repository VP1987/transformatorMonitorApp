<template>
  <div class="card">
    <header class="card-h">
      <CardControls
        :card="card"
        :totalCount="totalCount"
        :visibleCount="visibleCount"
        @open-settings="cardsStore.openSettings"
      />
    </header>

    <div class="card-body">
      <TransformerTable :items="items" />
      <TransformerChart v-if="card.settings.showChart" :items="items" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import type { CardModel } from "@/domain/entities/card/CardModel";
import { useTransformersStore } from "@/application/stores/transformers.store";
import { useCardsStore } from "@/application/stores/card.store";

import CardControls from "./CardControls.vue";
import TransformerTable from "./TransformerTable.vue";
import TransformerChart from "./TransformerChart.vue";

const props = defineProps<{ card: CardModel }>();

const transformersStore = useTransformersStore();
const cardsStore = useCardsStore();

const showConfirm = ref(false);

const items = computed(() =>
  cardsStore.apply(props.card, transformersStore.transformers)
);
const totalCount = computed(() => transformersStore.transformers.length);
const visibleCount = computed(() => items.value.length);
</script>

<style scoped>
.card {
  position: relative;
  background: var(--card-bg);
  border: 1px solid var(--border);
  border-radius: 16px;
  margin: 25px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.04);
  overflow: hidden;
}

.card-h {
  background: var(--header-bg);
  padding: 18px 20px;
  border-bottom: 1px solid var(--border);
}

.card-body {
  padding: 16px;
}

.confirm-overlay {
  position: absolute;
  inset: 0;
  background: rgba(2, 6, 23, 0.65);
  border: 1px solid var(--crit);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
}

.confirm-box {
  background: var(--card);
  border-radius: 12px;
  padding: 24px;
  width: 320px;
  text-align: center;
  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.4);
}

.confirm-box h3 {
  margin-bottom: 8px;
}

.confirm-box p {
  color: var(--muted);
  font-size: 14px;
}

.confirm-actions {
  margin-top: 20px;
  display: flex;
  gap: 12px;
  justify-content: center;
}

.btn {
  padding: 8px 16px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
}

.btn.cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--text);
}

.btn.danger {
  background: var(--crit);
  color: #fff;
}
</style>
