<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal">
      <header class="modal-h">
        <h2>Create Work Order</h2>
        <button class="close-btn" @click="$emit('close')">&times;</button>
      </header>

      <div class="modal-body">
        <!-- Asset Selection -->
        <div class="field-group">
          <label class="field-label">Select Transformer</label>
          <div class="select-wrapper">
            <select v-model="selectedAssetId" class="main-select">
              <option value="" disabled>Choose asset...</option>
              <option v-for="t in transformersStore.transformers" :key="t.id" :value="t.id">
                {{ t.name }} (#{{ t.assetId }})
              </option>
            </select>
          </div>
        </div>

        <!-- Auto Location -->
        <div v-if="selectedAsset" class="location-preview">
          <span class="loc-label">Location:</span>
          <span class="loc-value">{{ selectedAsset.region }}</span>
        </div>

        <!-- Priority & Team Row -->
        <div class="field-row">
          <div class="field-group flex-1">
            <label class="field-label">Urgency / Priority</label>
            <select v-model="priority" class="main-select">
              <option value="Low">Low</option>
              <option value="Medium">Medium</option>
              <option value="High">High</option>
              <option value="Critical">Critical</option>
            </select>
          </div>

          <div class="field-group flex-1">
            <label class="field-label">Assign To Team</label>
            <select v-model="assignedTeamId" class="main-select">
              <option value="">Auto-Assign Later</option>
              <option v-for="team in maintenanceStore.activeTeams" :key="team.id" :value="team.id">
                {{ team.name }}
              </option>
            </select>
          </div>
        </div>

        <!-- Description -->
        <div class="field-group">
          <label class="field-label">Task Description</label>
          <textarea 
            v-model="description" 
            placeholder="Describe the maintenance required..."
            rows="3"
          ></textarea>
        </div>
      </div>

      <footer class="modal-f">
        <button class="btn cancel" @click="$emit('close')">Cancel</button>
        <button 
          class="btn primary" 
          @click="submit" 
          :disabled="!selectedAssetId || !description"
        >
          Create Order
        </button>
      </footer>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useTransformersStore } from '@/application/stores/transformers.store';
import { useMaintenanceStore } from '@/application/stores/maintenance.store';

const emit = defineEmits<{ close: [] }>();

const transformersStore = useTransformersStore();
const maintenanceStore = useMaintenanceStore();

const selectedAssetId = ref<number | "">("");
const priority = ref("Medium");
const assignedTeamId = ref<number | "">("");
const description = ref("");

const selectedAsset = computed(() => {
  if (!selectedAssetId.value) {
    return null;
  }
  return transformersStore.transformers.find(t => t.id === selectedAssetId.value);
});

const submit = async () => {
  if (!selectedAssetId.value || !description.value) {
    return;
  }

  const success = await maintenanceStore.issueTicket(
    Number(selectedAssetId.value), 
    description.value, 
    priority.value
  );

  if (success && assignedTeamId.value) {
    // If team was selected, assign it immediately
    // Since we just reloaded, the new ticket should be in allTickets
    const newTicket = maintenanceStore.allTickets.find(t => t.description === description.value);
    if (newTicket) {
      await maintenanceStore.assignTicket(newTicket.id, Number(assignedTeamId.value));
    }
  }

  emit('close');
};
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
  z-index: 1100;
}

.modal {
  background: var(--card-bg);
  border: 1px solid var(--border);
  border-radius: 16px;
  width: 500px;
  box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.5);
  overflow: hidden;
}

.modal-h {
  padding: 20px 24px;
  border-bottom: 1px solid var(--border);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-body {
  padding: 24px;
}

.modal-f {
  padding: 20px 24px;
  background: var(--header-bg);
  border-top: 1px solid var(--border);
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.field-group { margin-bottom: 20px; }
.field-row { display: flex; gap: 16px; }
.flex-1 { flex: 1; }

.field-label {
  display: block;
  font-size: 11px;
  font-weight: 700;
  color: var(--muted);
  text-transform: uppercase;
  margin-bottom: 8px;
}

.main-select, textarea {
  width: 100%;
  background: var(--btn-bg);
  border: 1px solid var(--border);
  border-radius: 8px;
  color: var(--text);
  padding: 12px;
  font-size: 14px;
}

.location-preview {
  margin-top: -12px;
  margin-bottom: 20px;
  background: rgba(var(--accent-rgb, 79, 172, 254), 0.1);
  padding: 8px 12px;
  border-radius: 6px;
  display: flex;
  gap: 8px;
  font-size: 13px;
}

.loc-label { color: var(--muted); }
.loc-value { color: var(--accent); font-weight: 600; }

.btn {
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  border: none;
}

.btn.cancel { background: transparent; color: var(--text); border: 1px solid var(--border); }
.btn.primary { background: var(--accent); color: #020617; }
.btn.primary:disabled { opacity: 0.4; cursor: not-allowed; }

.close-btn {
  background: none;
  border: none;
  color: var(--muted);
  font-size: 24px;
  cursor: pointer;
}
</style>
