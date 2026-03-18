<template>
  <div class="table-wrap">
    <table>
      <thead>
        <tr>
          <th>ASSET ID</th>
          <th>REGION</th>
          <th>STATUS</th>
          <th>MAINTENANCE</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="t in items" :key="t.assetId">
          <td class="name">{{ t.name }}</td>
          <td class="region">{{ t.region }}</td>
          <td>
            <span :class="['badge', t.health.toLowerCase()]">
              {{ t.health.toUpperCase() }}
            </span>
          </td>
          <td>
            <button 
              class="issue-btn"
              @click="issueTicket(t)"
              :disabled="t.health === 'Good' || t.health === 'Excellent'"
            >
              Issue Ticket
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import type { Transformer } from "@/domain/entities/transformer/Transformer";
import { useMaintenanceStore } from "@/application/stores/maintenance.store";
import { useUIStore } from "@/application/stores/ui.store";

defineProps<{
  items: Transformer[];
}>();

const maintenanceStore = useMaintenanceStore();
const uiStore = useUIStore();

const issueTicket = async (t: Transformer) => {
  const priority = t.health === 'Critical' ? 'Critical' : 'High';
  const success = await maintenanceStore.issueTicket(t.id!, `Automated ticket for ${t.name} due to ${t.health} health status.`, priority);
  if (success) {
    uiStore.navigateTo('maintenance');
  }
};
</script>

<style scoped>
.table-wrap {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  font-size: 14px;
}

th {
  padding: 12px 18px;
  text-align: left;
  font-size: 11px;
  letter-spacing: 0.08em;
  color: var(--muted);
}

td {
  padding: 14px 18px;
  border-top: 1px solid var(--table-line);
}

.region {
  color: var(--info);
}

.badge {
  font-size: 11px;
  font-weight: 700;
  padding: 4px 10px;
  border-radius: 6px;
  border: 1px solid;
}

.issue-btn {
  background: var(--btn-bg);
  border: 1px solid var(--border);
  color: var(--text);
  font-size: 11px;
  padding: 4px 10px;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s;
}

.issue-btn:hover:not(:disabled) {
  background: var(--accent);
  color: white;
  border-color: var(--accent);
}

.issue-btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
}

.good { color: var(--info); border-color: var(--info); }
.excellent { color: var(--ok); border-color: var(--ok); }
.fair { color: var(--warn); border-color: var(--warn); }
.poor { color: var(--bad); border-color: var(--bad); }
.critical { color: var(--crit); border-color: var(--crit); }
</style>
