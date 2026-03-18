import { describe, it, expect } from 'vitest';
import { mapTransformerDtoToDomain } from '@/domain/mappers/transformerMapper';

describe('Transformer Mapper Tests', () => {
  it('should correctly map DTO to Domain entity', () => {
    const dto = {
      Id: 1,
      AssetId: 101,
      Name: "Test Transformer",
      Region: "North",
      lastReadings: [
        { Timestamp: "2026-03-18T10:00:00Z", VoltageValue: 13000 }
      ]
    };

    const domain = mapTransformerDtoToDomain(dto);

    expect(domain.assetId).toBe(101);
    expect(domain.name).toBe("Test Transformer");
    expect(domain.health).toBe("Critical"); // Voltage 13000 is < 14000
  });

  it('should map Good health for normal voltage', () => {
    const dto = {
      Id: 2,
      AssetId: 102,
      Name: "Healthy Asset",
      lastReadings: [
        { Timestamp: "2026-03-18T10:00:00Z", VoltageValue: 22500 }
      ]
    };

    const domain = mapTransformerDtoToDomain(dto);
    expect(domain.health).toBe("Good");
  });
});
