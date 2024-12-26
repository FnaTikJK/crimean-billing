export interface ISearchServiceRequestDTO {
  skip?: number;
  take?: number;
  excludedTemplateIds?: string[];
}
