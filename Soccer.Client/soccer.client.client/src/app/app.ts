import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

interface Country {
  id: number;
  name: string;
  enabled: boolean;
  createdAt: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class App implements OnInit {

  public countries: Country[] = [];

  public loading = true;

  public error = '';

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.loadCountries();
  }

  loadCountries(): void {

    this.http.get<Country[]>('/api/Countries')
      .subscribe({
        next: (data: Country[]) => {

          console.log('Países recibidos:', data);

          this.countries = data;
          this.loading = false;
        },

        error: (error: HttpErrorResponse) => {

          console.error('ERROR COMPLETO:', error);
          console.error('STATUS:', error.status);
          console.error('URL:', error.url);
          console.error('ERROR:', error.error);

          this.error = 'No se pudieron cargar los países.';
          this.loading = false;
        }
      });
  }
}
