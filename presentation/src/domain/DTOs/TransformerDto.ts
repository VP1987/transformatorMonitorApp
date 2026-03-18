export interface VoltageReadingDto {
  timestamp: string;
  voltage: string;
}

export interface TransformerDto {
  assetId: number;
  name: string;
  region: string;
  health: string;
  lastTenVoltageReadings: VoltageReadingDto[];
}
