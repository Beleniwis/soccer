import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Country {
  id: number;
  name: string;
  enabled: boolean;
  createdAt: string;
  teams: any[];
  leagues: any[];
}

export interface CreateCountry {
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class CountryService {

  private http = inject(HttpClient);

  getCountries(): Observable<Country[]> {
    return this.http.get<Country[]>('/api/Countries');
  }

  createCountry(country: CreateCountry): Observable<Country> {
    return this.http.post<Country>('/api/Countries', country);
  }

  updateCountry(
    id: number,
    country: CreateCountry
  ): Observable<void> {
    return this.http.put<void>(
      `/api/Countries/${id}`,
      country
    );
  }

  deleteCountry(id: number): Observable<void> {
    return this.http.delete<void>(
      `/api/Countries/${id}`
    );
  }
}
