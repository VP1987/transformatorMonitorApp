import { reactive } from "vue";
import type { Transformer } from "@/domain/entities/transformer/Transformer";

export interface TransformerAlert {
  assetId: number;
  name: string;
  currentVoltage: number;
  healthStatus: string;
}

class AlertService {
  private _alerts = reactive<Map<number, TransformerAlert>>(new Map());
  private _dismissedIds = reactive(new Set<number>());
  private readonly CRITICAL_STATES = ["Poor", "Critical"];

  public get alerts() {
    const active = new Map<number, TransformerAlert>();
    this._alerts.forEach((val, key) => {
      if (!this._dismissedIds.has(key)) {
        active.set(key, val);
      }
    });
    return active;
  }

  public processTransformers(transformers: Transformer[]): void {
    transformers.forEach((t) => {
      const isCritical = this.CRITICAL_STATES.includes(t.health);

      const readings = t.voltageReadings || [];
      const lastReading =
        readings.length > 0 ? readings[readings.length - 1] : null;

      if (isCritical) {
        this._alerts.set(t.assetId, {
          assetId: t.assetId,
          name: t.name,
          currentVoltage: lastReading ? lastReading.voltage : 0,
          healthStatus: t.health,
        });
      } else {
        this._alerts.delete(t.assetId);
        this._dismissedIds.delete(t.assetId);
      }
    });
  }

  public dismiss(assetId: number): void {
    this._dismissedIds.add(assetId);
  }
}

export const alertService = new AlertService();
