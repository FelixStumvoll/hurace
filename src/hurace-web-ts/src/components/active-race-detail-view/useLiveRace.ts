import { StartList } from '../../models/StartList';
import { RaceRanking } from '../../models/RaceRanking';
import { TimeDifference } from '../../models/TimeDifference';
import { useState, useEffect } from 'react';
import {
    getRankingForRace,
    getRemainingStartListForRace,
    getSplittimesForCurrentSkier,
    getCurrentSkierForRace
} from '../../common/api';

export const useLiveRace = (
    raceId: number
): [StartList[]?, RaceRanking[]?, StartList?, TimeDifference[]?] => {
    const [startList, setStartList] = useState<StartList[]>();
    const [ranking, setRanking] = useState<RaceRanking[]>();
    const [currentSkier, setCurrentSkier] = useState<StartList>();
    const [splitTimes, setSplitTimes] = useState<TimeDifference[]>();

    useEffect(() => {
        const fetchRaceData = async () => {
            setRanking(await getRankingForRace(raceId));
            setStartList(await getRemainingStartListForRace(raceId));
            let currSkier = await getCurrentSkierForRace(raceId);
            setCurrentSkier(currSkier);

            setSplitTimes(
                currSkier ? await getSplittimesForCurrentSkier(raceId) : []
            );
        };

        fetchRaceData();

        const interval = setInterval(async () => {
            await fetchRaceData();
        }, 15000);

        return () => clearInterval(interval);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return [startList, ranking, currentSkier, splitTimes];
};
