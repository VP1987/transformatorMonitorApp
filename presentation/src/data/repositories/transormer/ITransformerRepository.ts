import type { Transformer } from "@/domain/entities/transformer/Transformer.ts";

export interface ITransformerRepository {
  getAll(): Promise<Transformer[]>;
}
