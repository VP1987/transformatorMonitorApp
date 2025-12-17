<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal">
      <h2>Add Monitor</h2>

      <div class="field-group">
        <label class="field-label">Monitor Title</label>
        <input
          v-model="title"
          type="text"
          placeholder="e.g. Belgrade Main Station"
        />
      </div>

      <div class="field-group">
        <label class="field-label">REFRESH RATE (in seconds)</label>
        <div class="input-wrapper">
          <input
            v-model.number="refreshRate"
            type="number"
            min="1"
            class="refresh-input"
            placeholder="60"
          />
          <span class="unit-label">sec</span>
        </div>
      </div>

      <label class="all-toggle">
        <input type="checkbox" v-model="useAll" />
        <span>Monitor all transformers</span>
      </label>

      <div v-if="!useAll" class="selection-area">
        <input
          v-model="search"
          type="text"
          class="search-input"
          placeholder="Search by name, region or ID..."
        />

        <div class="list">
          <label
            v-for="t in filteredTransformers"
            :key="t.assetId"
            class="item"
          >
            <input type="checkbox" :value="t.assetId" v-model="selectedIds" />
            <span class="name">{{ t.name }}</span>
            <span class="meta">{{ t.region }} · #{{ t.assetId }}</span>
          </label>

          <div v-if="filteredTransformers.length === 0" class="empty">
            No transformers match your search.
          </div>
        </div>
      </div>

      <div class="actions">
        <button class="btn cancel" @click="$emit('close')">Cancel</button>
        <button
          class="btn primary"
          @click="save"
          :disabled="!title || (!useAll && selectedIds.length === 0)"
        >
          Save Configuration
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useCardsStore } from "@/application/stores/card.store";
import { useTransformersStore } from "@/application/stores/transformers.store";

const emit = defineEmits<{ close: [] }>();

const cardsStore = useCardsStore();
const transformersStore = useTransformersStore();

const title = ref("");
const useAll = ref(true);
const selectedIds = ref<number[]>([]);
const search = ref("");
const refreshRate = ref(10);

const filteredTransformers = computed(() => {
  const q = search.value.trim().toLowerCase();
  if (!q) return transformersStore.transformers;
  return transformersStore.transformers.filter(
    (t) =>
      t.name.toLowerCase().includes(q) ||
      t.region.toLowerCase().includes(q) ||
      String(t.assetId).includes(q)
  );
});

function save() {
  const idsToMonitor = useAll.value
    ? transformersStore.transformers.map((t) => t.assetId)
    : [...selectedIds.value];

  transformersStore.configureMonitoring(idsToMonitor, refreshRate.value);

  cardsStore.addCard({
    id: crypto.randomUUID(),
    title: title.value,
    type: "transformer",
    settings: {
      health: "All",
      limit: 10,
      sortBy: "health",
      sortDir: "asc",
      showChart: true,
      selectedTransformerIds: useAll.value ? [] : [...selectedIds.value],
      refreshInterval: refreshRate.value,
    },
  });

  emit("close");
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(2, 6, 23, 0.85);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal {
  background: var(--card);
  background-image: var(--card-bg);
  border: 1px solid var(--border);
  border-radius: 16px;
  padding: 32px;
  width: 460px;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
}

h2 {
  color: var(--text);
  font-size: 24px;
  margin-bottom: 24px;
}

.field-group {
  margin-bottom: 20px;
}

.field-label {
  display: block;
  font-size: 11px;
  font-weight: 700;
  color: var(--muted);
  text-transform: uppercase;
  margin-bottom: 8px;
}

input[type="text"],
.input-wrapper {
  width: 100%;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border);
  border-radius: 8px;
  color: var(--text);
  transition: border-color 0.2s;
}

input[type="text"] {
  padding: 12px 16px;
  font-size: 15px;
}

.input-wrapper {
  display: flex;
  align-items: center;
  padding-right: 12px;
}

.refresh-input {
  flex: 1;
  background: transparent !important;
  border: none !important;
  color: var(--text);
  padding: 12px;
  outline: none;
}

.unit-label {
  color: var(--muted);
  font-size: 13px;
}

.search-input {
  margin-bottom: 12px;
}

input:focus {
  outline: none;
  border-color: var(--accent);
}

.all-toggle {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 24px 0;
  cursor: pointer;
  color: var(--text);
  font-size: 14px;
}

.selection-area {
  border-top: 1px solid var(--border);
  padding-top: 20px;
}

.list {
  max-height: 200px;
  overflow-y: auto;
  margin-top: 12px;
  padding-right: 8px;
}

.item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 8px;
  border-radius: 6px;
  cursor: pointer;
  transition: background 0.2s;
}

.item:hover {
  background: rgba(255, 255, 255, 0.05);
}

.name {
  color: var(--text);
  font-weight: 500;
  font-size: 13px;
}
.meta {
  color: var(--muted);
  font-size: 12px;
  margin-left: auto;
}

.empty {
  text-align: center;
  color: var(--muted);
  font-size: 13px;
  padding: 20px 0;
}

.actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 32px;
}

.btn {
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
  font-size: 14px;
}

.btn.cancel {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--text);
}

.btn.primary {
  background: var(--accent);
  border: none;
  color: #020617;
}

.btn.primary:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
</style>
