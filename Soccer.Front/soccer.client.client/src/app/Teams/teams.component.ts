import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  TeamService,
  Team,
  CreateTeam
} from '../services/team.service';
import {
  CountryService,
  Country
} from '../services/country.service';

@Component({
  selector: 'app-teams',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './teams.component.html'
})
export class TeamsComponent implements OnInit {

  private teamService = inject(TeamService);
  private countryService = inject(CountryService);

  teams: Team[] = [];
  countries: Country[] = [];

  showForm = false;
  editingTeamId: number | null = null;

  teamName = '';
  selectedCountryId: number | null = null;

  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadCountries();
    this.loadTeams();
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

  loadTeams(): void {
    this.teamService.getTeams().subscribe({
      next: (teams) => {
        this.teams = teams;
      },
      error: (error) => {
        console.error('Error al obtener los equipos:', error);
        this.errorMessage = 'No se pudieron cargar los equipos.';
      }
    });
  }

  openCreateForm(): void {
    this.showForm = true;
    this.editingTeamId = null;

    this.teamName = '';
    this.selectedCountryId = null;

    this.errorMessage = '';
    this.successMessage = '';
  }

  openEditForm(team: Team): void {
    this.showForm = true;
    this.editingTeamId = team.id;

    this.teamName = team.name;
    this.selectedCountryId = team.countryId;

    this.errorMessage = '';
    this.successMessage = '';
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingTeamId = null;

    this.teamName = '';
    this.selectedCountryId = null;

    this.errorMessage = '';
  }

  saveTeam(): void {

    this.errorMessage = '';
    this.successMessage = '';

    const name = this.teamName.trim();

    if (!name) {
      this.errorMessage = 'Escribe el nombre del equipo.';
      return;
    }

    if (this.selectedCountryId === null) {
      this.errorMessage = 'Selecciona un país.';
      return;
    }

    const team: CreateTeam = {
      name,
      countryId: this.selectedCountryId
    };

    if (this.editingTeamId === null) {

      this.teamService.createTeam(team).subscribe({
        next: () => {
          this.successMessage = 'Equipo creado correctamente.';
          this.cancelForm();
          this.loadTeams();
        },
        error: (error) => {
          this.handleError(error);
        }
      });

    } else {

      this.teamService
        .updateTeam(this.editingTeamId, team)
        .subscribe({
          next: () => {
            this.successMessage = 'Equipo actualizado correctamente.';
            this.cancelForm();
            this.loadTeams();
          },
          error: (error) => {
            this.handleError(error);
          }
        });
    }
  }

  deleteTeam(team: Team): void {

    const confirmed = confirm(
      `¿Seguro que quieres eliminar el equipo "${team.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.teamService.deleteTeam(team.id).subscribe({
      next: () => {
        this.successMessage = 'Equipo eliminado correctamente.';
        this.loadTeams();
      },
      error: (error) => {
        this.handleError(error);
      }
    });
  }

  private handleError(error: any): void {

    console.error('Error:', error);

    if (error.status === 409) {
      this.errorMessage = 'Ya existe un equipo con ese nombre.';
    } else if (error.status === 400) {
      this.errorMessage =
        typeof error.error === 'string'
          ? error.error
          : 'Los datos enviados no son válidos.';
    } else if (error.status === 404) {
      this.errorMessage = 'El equipo no existe.';
    } else {
      this.errorMessage =
        'Ocurrió un error. Inténtalo nuevamente.';
    }
  }
}
