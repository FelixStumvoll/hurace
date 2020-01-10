import { Skier } from './Skier';

export interface RaceRanking {
    time?: Date;
    position?: number;
    timeToLeader?: Date;
    disqualified: boolean;
    startNumber: number;
    skier: Skier;
}
