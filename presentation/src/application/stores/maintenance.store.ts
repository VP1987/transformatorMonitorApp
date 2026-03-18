import { defineStore } from "pinia";
import { TeamApiRepository, TicketApiRepository } from "@/data/repositories/maintenance/MaintenanceRepositories";

export const useMaintenanceStore = defineStore("maintenance", {
  state: () => ({
    teams: [] as any[],
    activeTeams: [] as any[],
    allTickets: [] as any[],
    isLoaded: false,
    teamRepo: new TeamApiRepository(),
    ticketRepo: new TicketApiRepository()
  }),

  getters: {
    openTickets: (state) => {
      return state.allTickets.filter(t => t.status !== "Resolved");
    },
    resolvedTickets: (state) => {
      return state.allTickets.filter(t => t.status === "Resolved");
    }
  },

  actions: {
    async loadTickets(params?: { searchTerm?: string; teamId?: string; priority?: string; status?: string; sortDir?: string }) {
      try {
        const tickets = await this.ticketRepo.getAll(params);
        this.allTickets = tickets;
      } catch (error) {
        console.error("Maintenance load tickets error:", error);
      }
    },

    async load() {
      try {
        const [teams, activeTeams, tickets] = await Promise.all([
          this.teamRepo.getAll(),
          this.teamRepo.getActive(),
          this.ticketRepo.getAll()
        ]);

        this.teams = teams;
        this.activeTeams = activeTeams;
        this.allTickets = tickets;
        this.isLoaded = true;
      } catch (error) {
        console.error("Maintenance load error:", error);
      }
    },

    async issueTicket(transformerId: number, description: string, priority: string = "High") {
      try {
        const success = await this.ticketRepo.create(transformerId, description, priority);
        if (success) {
          await this.load();
        }
        return success;
      } catch (error) {
        return false;
      }
    },

    async assignTicket(ticketId: number, teamId: number) {
      try {
        const success = await this.ticketRepo.assign(ticketId, teamId);
        if (success) {
          await this.load();
        }
        return success;
      } catch (error) {
        return false;
      }
    },

    async resolveTicket(ticketId: number) {
      try {
        const success = await this.ticketRepo.resolve(ticketId);
        if (success) {
          await this.load();
        }
        return success;
      } catch (error) {
        console.error("Resolve error:", error);
        return false;
      }
    }
  }
});
