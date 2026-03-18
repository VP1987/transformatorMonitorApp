import { getApiUrl } from "@/application/services/ConfigService";

export class TeamApiRepository {
  async getAll() {
    const response = await fetch(`${getApiUrl()}/teams`);
    return response.ok ? await response.json() : [];
  }

  async getActive() {
    const response = await fetch(`${getApiUrl()}/teams/active`);
    return response.ok ? await response.json() : [];
  }
}

export class TicketApiRepository {
  async getAll() {
    const response = await fetch(`${getApiUrl()}/tickets`);
    return response.ok ? await response.json() : [];
  }

  async getOpen() {
    const response = await fetch(`${getApiUrl()}/tickets/open`);
    return response.ok ? await response.json() : [];
  }

  async create(transformerId: number, description: string, priority: string) {
    const response = await fetch(`${getApiUrl()}/tickets`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ transformerId, description, priority })
    });
    return response.ok;
  }

  async assign(ticketId: number, teamId: number) {
    const response = await fetch(`${getApiUrl()}/tickets/assign`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ ticketId, teamId })
    });
    return response.ok;
  }

  async resolve(ticketId: number) {
    const response = await fetch(`${getApiUrl()}/tickets/${ticketId}/resolve`, {
      method: "POST"
    });
    return response.ok;
  }
}
