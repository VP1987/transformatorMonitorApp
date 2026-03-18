<script setup lang="ts">
import { alertService } from "@/application/services/AlertService";
import { useUIStore } from "@/application/stores/ui.store";

const ui = useUIStore();

const getAlerts = () => alertService.alerts;

const handleDismiss = (assetId: number) => {
  alertService.dismiss(assetId);
};

const handleOpenQuickView = (assetId: number) => {
  ui.openQuickView(assetId);
};
</script>

<template>
  <div class="alert-overlay">
    <TransitionGroup name="fade">
      <div
        v-for="alert in getAlerts().values()"
        :key="alert.assetId"
        class="alert-card"
        :class="alert.healthStatus.toLowerCase()"
        @click="handleOpenQuickView(alert.assetId)"
      >
        <div class="alert-content">
          <div class="alert-header">
            <strong>⚠️ {{ alert.healthStatus }}: {{ alert.name }}</strong>
            <button
              class="dismiss-btn"
              @click.stop="handleDismiss(alert.assetId)"
            >
              ×
            </button>
          </div>
          <div class="alert-body">
            Napon: <strong>{{ alert.currentVoltage }}V</strong>
          </div>
        </div>
      </div>
    </TransitionGroup>
  </div>
</template>

<style scoped>
.alert-overlay {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.alert-card {
  color: white;
  padding: 12px 16px;
  border-radius: 6px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  min-width: 250px;
  transition: all 0.2s ease;
  cursor: pointer;
}

.alert-card:hover {
  transform: translateY(-2px);
  filter: brightness(1.1);
}

.alert-card.critical {
  background: #d32f2f;
}
.alert-card.poor {
  background: #ed6c02;
}

.alert-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 4px;
}

.dismiss-btn {
  background: none;
  border: none;
  color: white;
  font-size: 22px;
  line-height: 1;
  cursor: pointer;
  padding: 0 0 0 10px;
  opacity: 0.7;
}

.dismiss-btn:hover {
  opacity: 1;
}

.fade-enter-active,
.fade-leave-active {
  transition: all 0.4s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateX(40px);
}
</style>
