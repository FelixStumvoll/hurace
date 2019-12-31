import { Skier } from './Skier';
import { Race } from './Race';

export interface StartList {
    skier: Skier;
    skierId: number;
    race: Race;
    raceId: number;
    startNumber: number;
}
