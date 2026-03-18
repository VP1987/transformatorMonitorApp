import type { TransformerDto } from "@/domain/DTOs/TransformerDto";
export interface TransformerDataSource {
  subscribe(cb: (data: TransformerDto[]) => void): void;
  unsubscribe(): void;
}
