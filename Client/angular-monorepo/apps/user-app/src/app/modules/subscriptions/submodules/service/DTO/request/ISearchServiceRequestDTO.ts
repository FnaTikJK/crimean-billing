export interface ISearchServiceRequestDTO {
  skip?: number;
  take?: number;
  isTariffService?: boolean;
  excludedTemplateIds?: string[];
}
