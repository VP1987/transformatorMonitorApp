<template>
  <div class="chart-wrapper">
    <div class="ch-header">
      <span>VOLTAGE TREND</span>
    </div>
    <div ref="chartEl" class="chart-container"></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onBeforeUnmount, ref, watch } from "vue";
import * as echarts from "echarts";
import type { Transformer } from "@/domain/entities/transformer/Transformer";

const props = defineProps<{
  items: Transformer[];
}>();

const chartEl = ref<HTMLDivElement | null>(null);
let chart: echarts.ECharts | null = null;
let resizeObserver: ResizeObserver | null = null;

function buildOption() {
  const series = props.items.map((t) => ({
    name: t.name,
    type: "line",
    smooth: true,
    showSymbol: false,
    lineStyle: { width: 2 },
    data: (t.voltageReadings ?? []).map((v) => [v.timestamp, v.voltage]),
  }));

  return {
    backgroundColor: "transparent",
    tooltip: {
      trigger: "axis",
      backgroundColor: "#1e293b",
      borderColor: "#334155",
      textStyle: { color: "#f8fafc" },
    },
    legend: {
      bottom: 0,
      textStyle: { color: "#94a3b8" },
      itemWidth: 10,
      itemHeight: 10,
    },
    grid: {
      top: "10%",
      left: "3%",
      right: "4%",
      bottom: "15%",
      containLabel: true,
    },
    xAxis: {
      type: "time",
      axisLine: { lineStyle: { color: "#334155" } },
      axisLabel: { color: "#64748b" },
      splitLine: { show: false },
    },
    yAxis: {
      type: "value",
      axisLine: { show: false },
      axisLabel: { color: "#64748b" },
      splitLine: { lineStyle: { color: "rgba(51, 65, 85, 0.5)" } },
    },
    series,
  };
}

function render() {
  if (!chart) return;
  chart.setOption(buildOption(), true);
}

onMounted(() => {
  if (!chartEl.value) return;

  chart = echarts.init(chartEl.value);
  render();

  resizeObserver = new ResizeObserver(() => {
    chart?.resize();
  });
  resizeObserver.observe(chartEl.value);
});

watch(() => props.items, render, { deep: true });

onBeforeUnmount(() => {
  resizeObserver?.disconnect();
  chart?.dispose();
});
</script>

<style scoped>
.chart-wrapper {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.ch-header {
  padding: 8px 0;
  font-size: 11px;
  font-weight: 800;
  letter-spacing: 0.05em;
  color: var(--muted);
}

.chart-container {
  flex: 1;
  width: 100%;
  min-height: 200px;
}
</style>
