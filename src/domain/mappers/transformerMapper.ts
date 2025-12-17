import type { Transformer } from "../entities/transformer/Transformer";
import type { TransformerDto } from "../DTOs/TransformerDto";
import type { VoltageReading } from "../entities/voltage/VoltageReading";
import type { TransformerHealth } from "../entities/transformer/TransformerHealth";

function mapVoltageReading(dto: {
  timestamp: string;
  voltage: string;
}): VoltageReading {
  return {
    timestamp: new Date(dto.timestamp),
    voltage: Number(dto.voltage),
  };
}

function resolveHealth(voltage: number): TransformerHealth {
  if (voltage < 14000) return "Critical";
  if (voltage < 18000) return "Poor";
  if (voltage < 22000) return "Fair";
  return "Good";
}

export function mapTransformerDtoToDomain(dto: TransformerDto): Transformer {
  const readings = dto.lastTenVoltageReadings.map(mapVoltageReading);
  const last = readings[0];

  return {
    assetId: dto.assetId,
    name: dto.name,
    region: dto.region,
    health: last ? resolveHealth(last.voltage) : "Good",
    voltageReadings: readings,
  };
}
