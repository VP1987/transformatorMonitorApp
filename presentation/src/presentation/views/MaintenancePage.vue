<template>
  <div class="page-wrapper">
    <HeaderComponent />

    <main class="page">
      <div class="maintenance-layout">
        <!-- Actions Bar -->
        <div class="actions-bar">
          <button class="action-btn btn-primary" @click="showWorkOrderModal = true">
            <span class="plus">+</span> New Work Order
          </button>
          <button class="action-btn btn-secondary" @click="handleRefresh" :disabled="isRefreshing">
            {{ isRefreshing ? 'Refreshing...' : 'Refresh Status' }}
          </button>
        </div>

        <div class="draggable-container">
          <div 
            v-for="(section, index) in sectionOrder" 
            :key="section"
            class="draggable-section"
            draggable="true"
            @dragstart="onDragStart(index)"
            @dragover.prevent
            @drop="onDrop(index)"
          >
            <!-- Active Teams -->
            <div v-if="section === 'teams'" class="card">
              <header class="card-h">
                <div class="card-header-content">
                  <div class="header-left">
                    <span class="drag-icon">⋮⋮</span>
                    <h3>Active Teams</h3>
                  </div>
                  <span class="badge online-badge">{{ maintenanceStore.activeTeams.length }} Online</span>
                </div>
              </header>
              <div class="card-body">
                <div class="teams-grid">
                  <div v-for="team in maintenanceStore.activeTeams" :key="team.id" class="team-item-card">
                    <div class="team-header-row">
                      <span class="team-name">{{ team.name }}</span>
                      <span class="status-dot online"></span>
                    </div>
                    <div class="technicians-pills">
                      <span v-for="tech in team.technicians" :key="tech" class="pill">{{ tech }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Maintenance Backlog -->
            <div v-if="section === 'tickets'" class="card">
              <header class="card-h">
                <div class="card-header-content">
                  <div class="header-left">
                    <span class="drag-icon">⋮⋮</span>
                    <h3>Maintenance Backlog</h3>
                  </div>
                  
                  <div class="header-right">
                    <div class="tabs-group">
                      <button 
                        v-for="tab in ['All', 'Active', 'Resolved']" 
                        :key="tab"
                        :class="['tab-link', { active: activeTab === tab }]"
                        @click="activeTab = tab"
                      >
                        {{ tab }}
                      </button>
                    </div>
                    <button class="sort-toggle" @click="toggleSort">
                      {{ sortDir === 'desc' ? '▼' : '▲' }}
                    </button>
                  </div>
                </div>
              </header>

              <div class="card-body no-padding-top">
                <div class="inline-filters">
                  <div class="search-wrap">
                    <input v-model="searchQuery" type="text" placeholder="Search by asset or task..." />
                  </div>
                  <div class="team-filter-wrap">
                    <select v-model="teamFilter" class="select-field">
                      <option value="">All Teams</option>
                      <option v-for="team in maintenanceStore.teams" :key="team.id" :value="team.id">
                        {{ team.name }}
                      </option>
                    </select>
                  </div>
                  <div class="priority-filter-wrap">
                    <select v-model="priorityFilter" class="select-field">
                      <option value="">All Priorities</option>
                      <option value="Critical">Critical</option>
                      <option value="High">High</option>
                      <option value="Medium">Medium</option>
                      <option value="Low">Low</option>
                    </select>
                  </div>
                </div>

                <div class="table-scroll" style="position: relative; min-height: 200px;">
                  <div v-if="isLoadingTickets" class="spinner-overlay">
                    <div class="spinner"></div>
                  </div>
                  <table class="data-table">
                    <thead>
                      <tr>
                        <th style="width: 25%">Asset Name</th>
                        <th style="width: 15%">Priority</th>
                        <th style="width: 15%">Status</th>
                        <th style="width: 25%">Assignment</th>
                        <th style="width: 20%; text-align: right">Action</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="ticket in maintenanceStore.allTickets" :key="ticket.id">
                        <td class="asset-name-col">{{ ticket.transformerName }}</td>
                        <td>
                          <span :class="['prio-tag', ticket.priority.toLowerCase()]">
                            {{ ticket.priority }}
                          </span>
                        </td>
                        <td>{{ ticket.status }}</td>
                        <td>
                          <select 
                            v-if="ticket.status !== 'Resolved'"
                            class="table-select"
                            :value="ticket.assignedTeamId || ''"
                            @change="e => assign(ticket.id, (e.target as HTMLSelectElement).value)"
                          >
                            <option value="">Unassigned</option>
                            <option v-for="team in maintenanceStore.activeTeams" :key="team.id" :value="team.id">
                              {{ team.name }}
                            </option>
                          </select>
                          <span v-else class="team-label">{{ ticket.assignedTeamName }}</span>
                        </td>
                        <td style="text-align: right">
                          <button 
                            v-if="ticket.status !== 'Resolved' && ticket.assignedTeamId"
                            class="btn-resolve"
                            @click="handleResolve(ticket.id)"
                          >
                            Resolve
                          </button>
                          <span v-else-if="ticket.status === 'Resolved'" class="done-text">✓ Resolved</span>
                          <span v-else class="pending-label">Pending</span>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>

    <CreateWorkOrderModal v-if="showWorkOrderModal" @close="showWorkOrderModal = false" />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import HeaderComponent from "@/presentation/components/HeaderComponent.vue";
import CreateWorkOrderModal from "@/presentation/components/modals/CreateWorkOrderModal.vue";
import { useMaintenanceStore } from '@/application/stores/maintenance.store';
import { useUIStore } from '@/application/stores/ui.store';

const maintenanceStore = useMaintenanceStore();
const uiStore = useUIStore();
const showWorkOrderModal = ref(false);
const isRefreshing = ref(false);

const activeTab = ref('Active');
const searchQuery = ref('');
const teamFilter = ref('');
const priorityFilter = ref('');
const sortDir = ref<'asc' | 'desc'>('desc');

const sectionOrder = ref(['teams', 'tickets']);
const draggingIndex = ref<number | null>(null);
const isLoadingTickets = ref(false);
let searchTimeout: ReturnType<typeof setTimeout> | null = null;

const fetchFilteredTickets = async () => {
  isLoadingTickets.value = true;
  await maintenanceStore.loadTickets({
    searchTerm: searchQuery.value,
    teamId: teamFilter.value,
    priority: priorityFilter.value,
    status: activeTab.value === 'All' ? '' : activeTab.value,
    sortDir: sortDir.value
  });
  isLoadingTickets.value = false;
};

onMounted(async () => {
  await maintenanceStore.load(); // loads initial data
  const savedOrder = localStorage.getItem('maintenance_section_order');
  if (savedOrder && typeof savedOrder === 'string') {
    sectionOrder.value = JSON.parse(savedOrder);
  }
  fetchFilteredTickets();
});

watch([teamFilter, priorityFilter, activeTab, sortDir], () => {
  fetchFilteredTickets();
});

watch(searchQuery, () => {
  if (searchTimeout) clearTimeout(searchTimeout);
  searchTimeout = setTimeout(() => {
    fetchFilteredTickets();
  }, 400); // 400ms debounce
});

const handleRefresh = async () => {
  isRefreshing.value = true;
  await maintenanceStore.load();
  await fetchFilteredTickets();
  setTimeout(() => { isRefreshing.value = false; }, 500);
};

const toggleSort = () => {
  sortDir.value = sortDir.value === 'desc' ? 'asc' : 'desc';
};

const assign = async (ticketId: number, teamIdStr: string) => {
  const teamId = teamIdStr ? Number(teamIdStr) : null;
  if (teamId) {
    await maintenanceStore.assignTicket(ticketId, teamId);
    await fetchFilteredTickets();
  }
};

const handleResolve = async (ticketId: number) => {
  await maintenanceStore.resolveTicket(ticketId);
  await fetchFilteredTickets();
};

const onDragStart = (index: number) => {
  draggingIndex.value = index;
};

const onDrop = (index: number) => {
  if (draggingIndex.value !== null) {
    const item = sectionOrder.value.splice(draggingIndex.value, 1)[0];
    if (item !== undefined) {
      sectionOrder.value.splice(index, 0, item);
      draggingIndex.value = null;
      localStorage.setItem('maintenance_section_order', JSON.stringify(sectionOrder.value));
    }
  }
};
</script>

<style scoped>
.maintenance-layout { display: flex; flex-direction: column; padding: 0 25px; }

.spinner-overlay {
  position: absolute;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(255, 255, 255, 0.6);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
  border-radius: 8px;
}
:root[data-theme="dark"] .spinner-overlay { background: rgba(0, 0, 0, 0.4); }

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid var(--border);
  border-top: 4px solid #007bff; /* Blue spinner */
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.draggable-container {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.actions-bar {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 15px;
  margin-bottom: 20px;
}

.action-btn {
  width: 140px; height: 34px;
  display: flex; align-items: center; justify-content: center; gap: 8px;
  font-size: 12px; font-weight: 600; border-radius: 8px; cursor: pointer;
  box-sizing: border-box; transition: all 0.1s;
}

.action-btn:active { transform: scale(0.95); opacity: 0.85; }
.btn-primary { background: var(--accent); color: #020617; border: none; }
.btn-secondary { background: var(--btn-bg); color: var(--text); border: 1px solid var(--border); }

.card { background: var(--card-bg); border: 1px solid var(--border); border-radius: 16px; box-shadow: 0 8px 24px rgba(0, 0, 0, 0.04); overflow: hidden; }
.card-h { background: var(--header-bg); padding: 16px 20px; border-bottom: 1px solid var(--border); }

.card-header-content { display: flex; justify-content: space-between; align-items: center; width: 100%; }
.header-left { display: flex; align-items: center; gap: 12px; }
.header-right { display: flex; align-items: center; gap: 12px; }

.drag-icon { cursor: grab; color: var(--muted); font-size: 20px; padding: 4px; }

.badge.online-badge { background: var(--accent); color: #020617; padding: 4px 12px; border-radius: 12px; font-size: 11px; font-weight: 700; }

.tabs-group { display: flex; background: var(--btn-bg); padding: 3px; border-radius: 10px; border: 1px solid var(--border); }
.tab-link { padding: 5px 12px; border-radius: 7px; border: none; background: transparent; color: var(--muted); font-size: 11px; font-weight: 600; cursor: pointer; }
.tab-link.active { background: var(--bg); color: var(--text); }

.sort-toggle { background: var(--btn-bg); border: 1px solid var(--border); color: var(--muted); width: 28px; height: 28px; border-radius: 6px; cursor: pointer; font-size: 10px; display: flex; align-items: center; justify-content: center; }

.card-body { padding: 20px; }
.no-padding-top { padding-top: 0; }

.teams-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 16px; }
.team-item-card { background: var(--bg); border: 1px solid var(--border); padding: 16px; border-radius: 12px; display: flex; flex-direction: column; gap: 10px; }
.team-header-row { display: flex; justify-content: space-between; align-items: center; }
.team-name { font-weight: 600; font-size: 14px; }
.status-dot { width: 8px; height: 8px; border-radius: 50%; }
.status-dot.online { background: #28a745; box-shadow: 0 0 8px #28a745; }
.technicians-pills { display: flex; flex-wrap: wrap; gap: 6px; }
.pill { background: var(--btn-bg); padding: 4px 8px; border-radius: 6px; font-size: 11px; color: var(--text); }

.inline-filters { display: flex; align-items: center; gap: 12px; padding: 16px 0; border-bottom: 1px solid var(--border); margin-bottom: 16px; }
.search-wrap { flex: 1; }
.search-wrap input { width: 100%; background: var(--btn-bg); border: 1px solid var(--border); color: var(--text); padding: 8px 14px; border-radius: 8px; font-size: 13px; }
.select-field { background: var(--btn-bg); border: 1px solid var(--border); color: var(--text); padding: 8px 14px; border-radius: 8px; font-size: 13px; min-width: 180px; cursor: pointer; }

.data-table { width: 100%; border-collapse: collapse; }
.data-table th, .data-table td { padding: 14px 12px; text-align: left; border-bottom: 1px solid var(--border); font-size: 13px; }
.asset-name-col { font-weight: 600; color: var(--accent); }

.prio-tag { padding: 4px 8px; border-radius: 6px; font-size: 10px; font-weight: 700; text-transform: uppercase; }
.prio-tag.critical { background: var(--crit); color: white; }
.prio-tag.high { background: #ffa500; color: white; }
.prio-tag.medium { background: var(--accent); color: #020617; }

.table-select { width: 100%; background: var(--bg); border: 1px solid var(--border); color: var(--text); padding: 6px; border-radius: 6px; }

.btn-resolve { background: #28a745; color: white; border: none; padding: 6px 14px; border-radius: 8px; font-size: 12px; font-weight: 600; cursor: pointer; transition: all 0.1s; }
.btn-resolve:active { transform: scale(0.95); opacity: 0.9; }

.done-text { color: #28a745; font-weight: 600; font-size: 12px; }
.pending-label { color: var(--muted); font-size: 12px; }
.empty-state { text-align: center; padding: 40px; color: var(--muted); }

@media (max-width: 800px) {
  .inline-filters { flex-direction: column; align-items: stretch; }
  .select-field { min-width: 100%; }
}
</style>
