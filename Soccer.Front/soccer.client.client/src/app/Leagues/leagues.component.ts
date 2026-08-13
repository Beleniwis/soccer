import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { CountryService, Country } from '../services/country.service';
import { LeagueService,League,CreateLeague} from '../services/league.service';

@Component({
  selector: 'app-leagues',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './leagues.component.html'
})
export class LeaguesComponent implements OnInit {

  private leagueService = inject(LeagueService);
  private countryService = inject(CountryService);

  leagues: League[] = [];
  countries: Country[] = [];

  showForm = false;
  editingLeagueId: number | null = null;

  leagueName = '';
  selectedCountryId: number | null = null;
  startDate = '';
  endDate = '';

  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadCountries();
    this.loadLeagues();
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

  loadLeagues(): void {
    this.leagueService.getLeagues().subscribe({
      next: (leagues) => {
        this.leagues = leagues;
      },
      error: (error) => {
        console.error('Error al obtener las ligas:', error);
        this.errorMessage = 'No se pudieron cargar las ligas.';
      }
    });
  }

  openCreateForm(): void {
    this.showForm = true;
    this.editingLeagueId = null;

    this.leagueName = '';
    this.selectedCountryId = null;
    this.startDate = '';
    this.endDate = '';

    this.errorMessage = '';
    this.successMessage = '';
  }

  openEditForm(league: League): void {
    this.showForm = true;
    this.editingLeagueId = league.id;

    this.leagueName = league.name;
    this.selectedCountryId = league.countryId;

    this.startDate = league.startDate.substring(0, 10);
    this.endDate = league.endDate.substring(0, 10);

    this.errorMessage = '';
    this.successMessage = '';
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingLeagueId = null;

    this.leagueName = '';
    this.selectedCountryId = null;
    this.startDate = '';
    this.endDate = '';

    this.errorMessage = '';
  }

  saveLeague(): void {

    this.errorMessage = '';
    this.successMessage = '';

    const name = this.leagueName.trim();

    if (!name) {
      this.errorMessage = 'Escribe el nombre de la liga.';
      return;
    }

    if (this.selectedCountryId === null) {
      this.errorMessage = 'Selecciona un país.';
      return;
    }

    if (!this.startDate || !this.endDate) {
      this.errorMessage = 'Selecciona las fechas de la liga.';
      return;
    }

    if (this.startDate > this.endDate) {
      this.errorMessage =
        'La fecha de inicio no puede ser mayor que la fecha final.';
      return;
    }

    const league: CreateLeague = {
      name,
      countryId: this.selectedCountryId,
      startDate: this.startDate,
      endDate: this.endDate
    };

    if (this.editingLeagueId === null) {

      this.leagueService.createLeague(league).subscribe({
        next: () => {
          this.successMessage = 'Liga creada correctamente.';
          this.cancelForm();
          this.loadLeagues();
        },
        error: (error) => {
          this.handleError(error);
        }
      });

    } else {

      this.leagueService
        .updateLeague(this.editingLeagueId, league)
        .subscribe({
          next: () => {
            this.successMessage = 'Liga actualizada correctamente.';
            this.cancelForm();
            this.loadLeagues();
          },
          error: (error) => {
            this.handleError(error);
          }
        });
    }
  }

  deleteLeague(league: League): void {

    const confirmed = confirm(
      `¿Seguro que quieres eliminar la liga "${league.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.leagueService.deleteLeague(league.id).subscribe({
      next: () => {
        this.successMessage = 'Liga eliminada correctamente.';
        this.loadLeagues();
      },
      error: (error) => {
        this.handleError(error);
      }
    });
  }

  private handleError(error: any): void {

    console.error('Error:', error);

    if (error.status === 409) {
      this.errorMessage = 'Ya existe una liga con ese nombre.';
    } else if (error.status === 400) {
      this.errorMessage =
        typeof error.error === 'string'
          ? error.error
          : 'Los datos enviados no son válidos.';
    } else if (error.status === 404) {
      this.errorMessage = 'La liga no existe.';
    } else {
      this.errorMessage =
        'Ocurrió un error. Inténtalo nuevamente.';
    }
  }
}
