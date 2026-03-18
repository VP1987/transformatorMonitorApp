import type { Transformer } from "../entities/transformer/Transformer";
import { resolveTransformerHealth } from "../entities/transformer/Transformer";
import type { VoltageReading } from "../entities/voltage/VoltageReading";

function mapVoltageReading(dto: any): VoltageReading {
  return {
    timestamp: new Date(dto.timestamp || dto.Timestamp),
    voltage: Number(dto.voltageValue || dto.voltage || dto.VoltageValue),
  };
}

export function mapTransformerDtoToDomain(dto: any): Transformer {
  const rawReadings = dto.lastReadings || dto.lastTenVoltageReadings || [];
  const readings = rawReadings.map(mapVoltageReading);
  const last = readings[0];

  return {
    id: dto.id || dto.Id,
    assetId: dto.assetId || dto.AssetId,
    name: dto.name || dto.Name,
    region: dto.region || dto.Region,
    health: last ? resolveTransformerHealth(last.voltage) : "Good",
    voltageReadings: readings,
  };
}
