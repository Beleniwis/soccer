import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CountryService, Country } from './services/country.service';
import { TeamsComponent } from './Teams/teams.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    FormsModule,
    TeamsComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {

  private countryService = inject(CountryService);

  countries: Country[] = [];

  showForm = false;
  editingCountryId: number | null = null;
  countryName = '';

  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadCountries();
  }

  loadCountries(): void {
    this.countryService.getCountries().subscribe({
      next: (countries) => {
        this.countries = countries;
      },
      error: (error) => {
        console.error('Error al obtener los países:', error);
        this.errorMessage = 'No se pudieron cargar los países.';
      }
    });
  }

  openCreateForm(): void {
    this.showForm = true;
    this.editingCountryId = null;
    this.countryName = '';
    this.errorMessage = '';
    this.successMessage = '';
  }

  openEditForm(country: Country): void {
    this.showForm = true;
    this.editingCountryId = country.id;
    this.countryName = country.name;
    this.errorMessage = '';
    this.successMessage = '';
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingCountryId = null;
    this.countryName = '';
    this.errorMessage = '';
  }

  saveCountry(): void {
    this.errorMessage = '';
    this.successMessage = '';

    const name = this.countryName.trim();

    if (!name) {
      this.errorMessage = 'Escribe el nombre del país.';
      return;
    }

    if (this.editingCountryId === null) {

      this.countryService.createCountry({ name }).subscribe({
        next: () => {
          this.successMessage = 'País creado correctamente.';
          this.cancelForm();
          this.loadCountries();
        },
        error: (error) => {
          this.handleError(error);
        }
      });

    } else {

      this.countryService
        .updateCountry(this.editingCountryId, { name })
        .subscribe({
          next: () => {
            this.successMessage = 'País actualizado correctamente.';
            this.cancelForm();
            this.loadCountries();
          },
          error: (error) => {
            this.handleError(error);
          }
        });
    }
  }

  deleteCountry(country: Country): void {

    const confirmed = confirm(
      `¿Seguro que quieres eliminar el país "${country.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.countryService.deleteCountry(country.id).subscribe({
      next: () => {
        this.successMessage = 'País eliminado correctamente.';
        this.loadCountries();
      },
      error: (error) => {
        this.handleError(error);
      }
    });
  }

  private handleError(error: any): void {

    console.error('Error:', error);

    if (error.status === 409) {
      this.errorMessage = 'Ya existe un país con ese nombre.';
    } else if (error.status === 400) {
      this.errorMessage =
        typeof error.error === 'string'
          ? error.error
          : 'Los datos enviados no son válidos.';
    } else if (error.status === 404) {
      this.errorMessage = 'El país no existe.';
    } else {
      this.errorMessage = 'Ocurrió un error. Inténtalo nuevamente.';
    }
  }
}
