<template>
  <div class="card-controls">
    <div class="header-top">
      <div class="title-wrap">
        <h3 class="title">{{ card.title }}</h3>
        <div class="count-inline">
          Showing <strong>{{ visibleCount }}</strong> of
          <strong>{{ totalCount }}</strong>
        </div>
      </div>

      <button class="kebab-btn" @click="emit('open-settings', card.id)">
        ⋮
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { CardModel } from "@/domain/entities/card/CardModel";

defineProps<{
  card: CardModel;
  totalCount: number;
  visibleCount: number;
}>();

const emit = defineEmits<{
  (e: "open-settings", cardId: string): void;
}>();
</script>

<style scoped>
.card-controls {
  position: relative;
}

.header-top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.title-wrap {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.title {
  font-size: 16px;
  font-weight: 600;
}

.count-inline {
  font-size: 12px;
  color: var(--muted);
}

.kebab-btn {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  background: var(--surface);
  border: 1px solid var(--border);
  font-size: 18px;
  cursor: pointer;
}
</style>
