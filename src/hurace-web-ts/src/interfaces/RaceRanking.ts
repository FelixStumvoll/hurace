import { StartList } from "./StartList";

export interface RaceRanking{
    startList : StartList;
    time?: Date;
    position?: number;
    timeToLeader?: Date;
    disqualified: boolean;
}