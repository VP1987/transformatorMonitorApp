<template>
  <div class="page-wrapper">
    <HeaderComponent />

    <main class="page">
      <!-- Actions Bar (Same as Maintenance) -->
      <div class="actions-bar">
        <button class="action-btn btn-primary" @click="showAddModal = true">
          <span class="plus">+</span> Add New Monitor
        </button>
      </div>

      <div class="cards-grid">
        <CardComponent
          v-for="card in cardsStore.cards"
          :key="card.id"
          :card="card"
        />
      </div>
    </main>

    <AddCardModal v-if="showAddModal" @close="showAddModal = false" />
    <CardSettingsModal />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import HeaderComponent from "@/presentation/components/HeaderComponent.vue";
import CardComponent from "@/presentation/components/card/CardComponent.vue";
import AddCardModal from "@/presentation/components/modals/AddCardModal.vue";
import CardSettingsModal from "@/presentation/components/modals/CardSettingsModal.vue";
import { useCardsStore } from "@/application/stores/card.store";
import { useTransformersStore } from "@/application/stores/transformers.store";

const cardsStore = useCardsStore();
const transformersStore = useTransformersStore();
const showAddModal = ref(false);

onMounted(async () => {
  await cardsStore.load();
  await transformersStore.load();
});
</script>

<style scoped>
.actions-bar {
  display: flex;
  justify-content: flex-end;
  padding: 0 25px;
  margin-top: 15px;
  margin-bottom: 5px;
}

.action-btn {
  width: 140px;
  height: 34px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  font-size: 12px;
  font-weight: 600;
  border-radius: 8px;
  cursor: pointer;
  border: none;
  background: var(--accent);
  color: #020617;
  transition: all 0.1s;
}

.action-btn:active {
  transform: scale(0.95);
  opacity: 0.85;
}

.cards-grid {
  display: flex;
  flex-direction: column;
}
</style>
