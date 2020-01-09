import { StartList } from './StartList';

export interface TimeData {
    skierId: number;
    raceId: number;
    sensorId: number;
    time: number;
    skierEventId: number;
    startList: StartList;
}
