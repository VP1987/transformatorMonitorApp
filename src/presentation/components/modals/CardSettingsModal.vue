<template>
  <teleport to="body">
    <div v-if="card" class="overlay" @click.self="close">
      <div class="modal">
        <div class="modal-header">
          <h3>Monitor settings – {{ card.title }}</h3>
          <div class="subtitle">
            {{
              confirmingDelete
                ? "This action cannot be undone"
                : "Configure filters and behavior"
            }}
          </div>
        </div>

        <div v-if="!confirmingDelete" class="form">
          <div class="field-group">
            <label class="field-label">REFRESH RATE (in seconds)</label>
            <div class="input-wrapper">
              <input
                class="refresh-input"
                type="number"
                min="1"
                :value="card.settings.refreshInterval"
                @input="setRefreshInterval"
              />
              <span class="unit-label">sec</span>
            </div>
          </div>

          <div class="grid">
            <div class="field-group">
              <label class="field-label">Health Filter</label>
              <select
                class="field"
                :value="card.settings.health"
                @change="set('health', $event)"
              >
                <option value="All">All health</option>
                <option value="Good">Good</option>
                <option value="Fair">Fair</option>
                <option value="Poor">Poor</option>
                <option value="Critical">Critical</option>
              </select>
            </div>

            <div class="field-group">
              <label class="field-label">Sort By</label>
              <select
                class="field"
                :value="card.settings.sortBy"
                @change="set('sortBy', $event)"
              >
                <option value="id">Asset ID</option>
                <option value="name">Name</option>
                <option value="region">Region</option>
                <option value="health">Health</option>
              </select>
            </div>
          </div>

          <div class="field-group">
            <label class="field-label">Sort Direction</label>
            <select
              class="field"
              :value="card.settings.sortDir"
              @change="set('sortDir', $event)"
            >
              <option value="asc">Ascending</option>
              <option value="desc">Descending</option>
            </select>
          </div>

          <div class="field-group">
            <label class="field-label">Limit Results</label>
            <input
              class="field"
              type="number"
              min="1"
              :value="card.settings.limit"
              @input="setLimit"
            />
          </div>

          <label class="checkbox-label">
            <input
              type="checkbox"
              :checked="card.settings.showChart"
              @change="set('showChart', $event)"
            />
            <span>Show chart</span>
          </label>
        </div>

        <div v-else class="confirm">
          <p>
            Are you sure you want to delete
            <strong>{{ card.title }}</strong
            >?
          </p>
        </div>

        <div class="actions">
          <template v-if="!confirmingDelete">
            <button class="btn danger-outline" @click="confirmingDelete = true">
              Delete card
            </button>
            <div class="right-actions">
              <button class="btn ghost" @click="close">Close</button>
            </div>
          </template>

          <template v-else>
            <button class="btn danger" @click="deleteCard">Yes, delete</button>
            <button class="btn ghost" @click="confirmingDelete = false">
              Cancel
            </button>
          </template>
        </div>
      </div>
    </div>
  </teleport>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useCardsStore } from "@/application/stores/card.store";
import { useTransformersStore } from "@/application/stores/transformers.store";

const cards = useCardsStore();
const transformersStore = useTransformersStore();
const confirmingDelete = ref(false);

const card = computed(
  () => cards.cards.find((c) => c.id === cards.activeCardId) ?? null
);

function close() {
  confirmingDelete.value = false;
  cards.closeSettings();
}

function deleteCard() {
  if (!card.value) return;
  cards.confirmDelete();
  close();
}

function set(key: string, e: Event) {
  if (!card.value) return;
  const t = e.target as HTMLInputElement;

  cards.updateCardSetting(
    card.value.id,
    key,
    t.type === "checkbox" ? t.checked : t.value
  );
}

function setLimit(e: Event) {
  if (!card.value) return;
  const v = Number((e.target as HTMLInputElement).value);

  cards.updateCardSetting(
    card.value.id,
    "limit",
    Number.isFinite(v) && v >= 1 ? v : 1
  );
}

function setRefreshInterval(e: Event) {
  if (!card.value) return;
  const v = Number((e.target as HTMLInputElement).value);
  const interval = Number.isFinite(v) && v >= 1 ? v : 1;

  cards.updateCardSetting(card.value.id, "refreshInterval", interval);

  const idsToUpdate = card.value.settings.selectedTransformerIds?.length
    ? card.value.settings.selectedTransformerIds
    : transformersStore.transformers.map((t) => t.assetId);

  transformersStore.configureMonitoring(idsToUpdate, interval);
}
</script>

<style scoped>
.overlay {
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
  width: 460px;
  background: var(--card);
  background-image: var(--card-bg);
  border: 1px solid var(--border);
  border-radius: 16px;
  padding: 32px;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
}

.modal-header {
  margin-bottom: 24px;
}

.modal-header h3 {
  margin: 0;
  font-size: 20px;
  font-weight: 600;
  color: var(--text);
}

.subtitle {
  font-size: 13px;
  color: var(--muted);
  margin-top: 4px;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.field-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.field-label {
  font-size: 10px;
  font-weight: 700;
  color: var(--muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.field,
.input-wrapper {
  width: 100%;
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border);
  border-radius: 8px;
  color: var(--text);
  font-size: 14px;
  transition: border-color 0.2s;
}

.field {
  height: 40px;
  padding: 0 14px;
}

.field:focus,
.input-wrapper:focus-within {
  outline: none;
  border-color: var(--accent);
}

.input-wrapper {
  display: flex;
  align-items: center;
  height: 40px;
  padding-right: 12px;
}

.refresh-input {
  flex: 1;
  background: transparent !important;
  border: none !important;
  color: var(--text);
  padding: 0 14px;
  height: 100%;
  outline: none;
}

.unit-label {
  color: var(--muted);
  font-size: 12px;
}

.grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.checkbox-label {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-top: 8px;
  cursor: pointer;
  color: var(--text);
  font-size: 14px;
}

.confirm {
  padding: 24px 0;
  font-size: 15px;
  color: var(--text);
}

.actions {
  margin-top: 32px;
  padding-top: 20px;
  border-top: 1px solid var(--border);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.right-actions {
  display: flex;
  gap: 12px;
}

.btn {
  height: 40px;
  padding: 0 20px;
  border-radius: 8px;
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s;
}

.btn.ghost {
  background: transparent;
  border: 1px solid var(--border);
  color: var(--text);
}

.btn.danger-outline {
  background: transparent;
  border: 1px solid var(--crit);
  color: var(--crit);
}

.btn.danger {
  background: var(--crit);
  border: none;
  color: white;
}

select option {
  background: var(--card);
  color: var(--text);
}
</style>
