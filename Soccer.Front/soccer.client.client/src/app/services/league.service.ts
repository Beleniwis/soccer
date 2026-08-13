import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface League {
  id: number;
  name: string;
  countryId: number;
  startDate: string;
  endDate: string;
  enabled: boolean;
  createdAt: string;
  country?: {
    id: number;
    name: string;
  };
  teamLeagues?: any[];
}

export interface CreateLeague {
  name: string;
  countryId: number;
  startDate: string;
  endDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class LeagueService {

  private http = inject(HttpClient);

  getLeagues(): Observable<League[]> {
    return this.http.get<League[]>('/api/Leagues');
  }

  createLeague(league: CreateLeague): Observable<League> {
    return this.http.post<League>('/api/Leagues', league);
  }

  updateLeague(
    id: number,
    league: CreateLeague
  ): Observable<void> {
    return this.http.put<void>(
      `/api/Leagues/${id}`,
      league
    );
  }

  deleteLeague(id: number): Observable<void> {
    return this.http.delete<void>(
      `/api/Leagues/${id}`
    );
  }
}
