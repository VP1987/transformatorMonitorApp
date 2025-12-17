<template>
  <div class="table-wrap">
    <table>
      <thead>
        <tr>
          <th>ASSET ID</th>
          <th>REGION</th>
          <th>STATUS</th>
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
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import type { Transformer } from "@/domain/entities/transformer/Transformer";

defineProps<{
  items: Transformer[];
}>();
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

.sel {
  width: 60px;
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

.good {
  color: var(--info);
  border-color: var(--info);
}
.excellent {
  color: var(--ok);
  border-color: var(--ok);
}
.fair {
  color: var(--warn);
  border-color: var(--warn);
}
.poor {
  color: var(--bad);
  border-color: var(--bad);
}
.critical {
  color: var(--crit);
  border-color: var(--crit);
}
</style>
