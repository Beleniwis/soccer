import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Team {
  id: number;
  name: string;
  countryId: number;
  enabled: boolean;
  createdAt: string;
  country?: {
    id: number;
    name: string;
  };
  players?: any[];
  teamLeagues?: any[];
}

export interface CreateTeam {
  name: string;
  countryId: number;
}

@Injectable({
  providedIn: 'root'
})
export class TeamService {

  private http = inject(HttpClient);

  getTeams(): Observable<Team[]> {
    return this.http.get<Team[]>('/api/Teams');
  }

  createTeam(team: CreateTeam): Observable<Team> {
    return this.http.post<Team>('/api/Teams', team);
  }

  updateTeam(
    id: number,
    team: CreateTeam
  ): Observable<void> {
    return this.http.put<void>(
      `/api/Teams/${id}`,
      team
    );
  }

  deleteTeam(id: number): Observable<void> {
    return this.http.delete<void>(
      `/api/Teams/${id}`
    );
  }
}
