import type { VoltageReading } from "../voltage/VoltageReading";
import type { TransformerHealth } from "./TransformerHealth";

export interface Transformer {
  assetId: number;
  name: string;
  region: string;
  health: TransformerHealth;
  voltageReadings: VoltageReading[];
}
