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
  async getAll(params?: { searchTerm?: string; teamId?: string; priority?: string; status?: string; sortDir?: string }) {
    let url = `${getApiUrl()}/tickets`;
    if (params) {
      const query = new URLSearchParams();
      if (params.searchTerm) query.append('searchTerm', params.searchTerm);
      if (params.teamId) query.append('teamId', params.teamId);
      if (params.priority) query.append('priority', params.priority);
      if (params.status) query.append('status', params.status);
      if (params.sortDir) query.append('sortDir', params.sortDir);
      
      const queryString = query.toString();
      if (queryString) {
        url += `?${queryString}`;
      }
    }
    const response = await fetch(url);
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
