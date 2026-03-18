import * as signalR from "@microsoft/signalr";
import type { TransformerDataSource } from "@/application/datasources/TransformerDataSource";
import { getHubUrl } from "@/application/services/ConfigService";

export class SignalRTransformerDataSource implements TransformerDataSource {
  private connection: signalR.HubConnection | null = null;

  public async subscribe(cb: (data: any) => void): Promise<void> {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(getHubUrl())
      .withAutomaticReconnect()
      .build();

    this.connection.on("ReceiveVoltageUpdate", (update) => {
      cb(update);
    });

    try {
      await this.connection.start();
    } catch (err) {
      console.error("SignalR Connection Error: ", err);
    }
  }

  public async unsubscribe(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
    }
  }
}
