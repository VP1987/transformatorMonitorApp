<template>
  <teleport to="body">
    <div
      v-if="ui.isQuickViewOpen && transformer"
      class="overlay"
      @click.self="ui.closeQuickView"
    >
      <div class="modal">
        <header class="modal-header">
          <div class="title-wrap">
            <div
              class="status-indicator"
              :class="transformer.health.toLowerCase()"
            ></div>
            <h3>{{ transformer.name }}</h3>
          </div>
          <span class="asset-tag">ASSET ID: #{{ transformer.assetId }}</span>
        </header>

        <div class="content">
          <div class="quick-stats">
            <div class="stat-box">
              <label>Current Voltage</label>
              <div class="stat-value">{{ currentVoltage }}<span>V</span></div>
            </div>
            <div class="stat-box">
              <label>Region</label>
              <div class="stat-value secondary">{{ transformer.region }}</div>
            </div>
          </div>

          <div class="visual-section">
            <div class="chart-container">
              <TransformerChart :items="transformer ? [transformer] : []" />
            </div>
          </div>

          <div class="work-order-area">
            <div v-if="!showForm" class="action-trigger">
              <button class="btn-action primary" @click="showForm = true">
                CREATE WORK ORDER
              </button>
            </div>

            <div v-else class="action-form">
              <div class="form-header">
                <h4>Dispatch Field Team</h4>
              </div>

              <div class="input-group">
                <label>Priority Level</label>
                <select v-model="order.priority" class="dark-input">
                  <option value="critical">Critical - Immediate Action</option>
                  <option value="high">High Priority</option>
                  <option value="medium">Medium Priority</option>
                </select>
              </div>

              <div class="input-group">
                <label>Task Instructions</label>
                <textarea
                  v-model="order.note"
                  class="dark-input area"
                  placeholder="Describe the issue..."
                ></textarea>
              </div>

              <div class="form-buttons">
                <button class="btn-text" @click="showForm = false">
                  Cancel
                </button>
                <button
                  class="btn-action primary"
                  @click="submitOrder"
                  :disabled="isSending"
                >
                  {{ isSending ? "DISPATCHING..." : "CONFIRM DISPATCH" }}
                </button>
              </div>
            </div>
          </div>
        </div>

        <button class="close-x" @click="ui.closeQuickView">✕</button>
      </div>
    </div>
  </teleport>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useUIStore } from "@/application/stores/ui.store";
import { useTransformersStore } from "@/application/stores/transformers.store";
import TransformerChart from "@/presentation/components/card/TransformerChart.vue";

const ui = useUIStore();
const transformersStore = useTransformersStore();

const showForm = ref(false);
const isSending = ref(false);
const order = ref({ priority: "high", note: "" });

const transformer = computed(() =>
  transformersStore.transformers.find(
    (t) => t.assetId === ui.selectedTransformerId
  )
);

const currentVoltage = computed(() => {
  return transformer.value?.voltageReadings?.[0]?.voltage ?? "N/A";
});

function submitOrder() {
  isSending.value = true;
  setTimeout(() => {
    isSending.value = false;
    showForm.value = false;
    ui.closeQuickView();
  }, 1000);
}
</script>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(2, 6, 23, 0.85);
  backdrop-filter: blur(12px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 99999;
  padding: 20px;
}

.modal {
  background: var(--card);
  border: 1px solid var(--border);
  border-radius: 24px;
  width: 100%;
  max-width: 520px;
  max-height: 90vh;
  padding: 40px;
  position: relative;
  box-shadow: 0 40px 80px rgba(0, 0, 0, 0.7);
  color: var(--text);
  overflow-y: auto;
  display: flex;
  flex-direction: column;
}

.modal-header {
  margin-bottom: 32px;
  flex-shrink: 0;
}

.title-wrap {
  display: flex;
  align-items: center;
  gap: 12px;
}

.status-indicator {
  width: 12px;
  height: 12px;
  border-radius: 50%;
}

.status-indicator.good {
  background: var(--ok);
}
.status-indicator.critical {
  background: var(--crit);
}
.status-indicator.fair {
  background: var(--warn);
}
.status-indicator.poor {
  background: var(--bad);
}

h3 {
  font-size: 22px;
  color: var(--text);
  margin: 0;
}

.asset-tag {
  display: block;
  font-size: 11px;
  color: var(--muted);
  margin-top: 4px;
}

.content {
  flex: 1;
}

.quick-stats {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 24px;
  margin-bottom: 32px;
}

.stat-box label {
  display: block;
  font-size: 10px;
  color: var(--muted);
  text-transform: uppercase;
  margin-bottom: 6px;
}

.stat-value {
  font-size: 28px;
  font-weight: 700;
  color: var(--text);
}

.stat-value span {
  font-size: 14px;
  color: var(--muted);
  margin-left: 4px;
}

.stat-value.secondary {
  font-size: 20px;
  color: var(--info);
}

.visual-section {
  background: var(--bg);
  border: 1px solid var(--border);
  border-radius: 16px;
  min-height: 220px;
  margin-bottom: 32px;
  padding: 16px;
  display: flex;
  flex-direction: column;
}

.chart-container {
  flex: 1;
  width: 100%;
  height: 100%;
  min-height: 180px;
}

.work-order-area {
  border-top: 1px solid var(--border);
  padding-top: 32px;
  flex-shrink: 0;
}

.btn-action {
  height: 48px;
  border-radius: 12px;
  font-weight: 700;
  cursor: pointer;
  border: none;
}

.btn-action.primary {
  background: var(--info);
  color: var(--bg);
  width: 100%;
}

.input-group label {
  display: block;
  font-size: 11px;
  font-weight: 700;
  color: var(--muted);
  margin-bottom: 8px;
  text-transform: uppercase;
}

.dark-input {
  width: 100%;
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: 10px;
  color: var(--text);
  padding: 12px;
  margin-bottom: 16px;
  font-family: inherit;
}

.area {
  height: 100px;
  resize: none;
}

.form-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 20px;
  margin-top: 20px;
}

.btn-text {
  background: none;
  border: none;
  color: var(--muted);
  cursor: pointer;
  font-weight: 600;
}

.close-x {
  position: absolute;
  top: 24px;
  right: 24px;
  background: none;
  border: none;
  color: var(--muted);
  font-size: 24px;
  cursor: pointer;
  line-height: 1;
}

.close-x:hover {
  color: var(--text);
}
</style>
