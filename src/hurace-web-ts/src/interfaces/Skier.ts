import { Country } from './Country';
import { Gender } from './Gender';

export interface Skier {
    id: number;
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    countryId: number;
    country: Country;
    genderId: number;
    gender: Gender;
    imageUrl?: string;
}
