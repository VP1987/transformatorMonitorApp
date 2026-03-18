import type { VoltageReading } from "../voltage/VoltageReading";
import type { TransformerHealth } from "./TransformerHealth";

export interface Transformer {
  id?: number;
  assetId: number;
  name: string;
  region: string;
  health: TransformerHealth;
  voltageReadings: VoltageReading[];
}

export function resolveTransformerHealth(voltage: number): TransformerHealth {
  if (voltage < 14000) return "Critical";
  if (voltage < 18000) return "Poor";
  if (voltage < 22000) return "Fair";
  return "Good";
}
