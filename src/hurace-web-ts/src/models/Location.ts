import { Country } from './Country';

export interface Location {
    id: number;
    locationName: string;
    countryId: number;
    country: Country;
}
