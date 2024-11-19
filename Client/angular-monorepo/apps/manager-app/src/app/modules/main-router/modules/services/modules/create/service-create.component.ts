import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject } from "@angular/core";
import { RouterModule } from "@angular/router";
import ListFilterComponent from '../../../../../shared/components/list-filter/list-filter.component';
import { ServicesService } from "../../services/services.service";

@Component({
  selector: 'app-service-create',
  standalone: true,
  imports: [CommonModule, RouterModule, ListFilterComponent],
  templateUrl: './service-create.component.html',
  styleUrl: './service-create.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export default class ServiceCreateComponent {
  servicesS = inject(ServicesService)
}
