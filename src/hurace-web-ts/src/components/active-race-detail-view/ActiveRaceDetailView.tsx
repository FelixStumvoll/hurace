import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { RaceRanking } from '../../models/RaceRanking';
import { Race } from '../../models/Race';
import { StartList } from '../../models/StartList';
import { TimeDifference } from '../../models/TimeDifference';
import { RaceDetailPanel } from '../race-view/RaceDetailPanel';
import { StartListView } from '../race-view/list-views/startlist-view/StartListView';
import { RankingView } from '../race-view/list-views/ranking-view/RankingView';
import {
    getRaceDetails,
    getRankingForRace,
    getRemainingStartListForRace,
    getSplittimesForCurrentSkier,
    getCurrentSkierForRace
} from '../../common/api';
import { CurrentSkierPanel } from './CurrentSkierPanel';
import { SplitTimeListView } from './split-time-view/SplitTimeListView';

const ActiveRacePanel = styled.div`
    display: flex;
    flex-direction: column;
    height: 100%;
`;

const Details = styled.div`
    display: flex;
`;

const DetailPanelWrapper = styled.div`
    height: fit-content;
    margin-right: ${props => props.theme.gap};
`;

const Lists = styled.div`
    display: grid;
    gap: ${props => props.theme.gap};
    margin-top: ${props => props.theme.gap};
    grid-template-columns: 1fr 1fr 1fr;
`;

export const ActiveRaceDetailView: React.FC<{ raceId: number }> = ({
    raceId
}) => {
    const [race, setRace] = useState<Race>();
    const [startList, setStartList] = useState<StartList[]>();
    const [ranking, setRanking] = useState<RaceRanking[]>();
    const [currentSkier, setCurrentSkier] = useState<StartList>();
    const [splitTimes, setSplitTimes] = useState<TimeDifference[]>();

    useEffect(() => {
        const loadRace = async () => setRace(await getRaceDetails(raceId));

        const fetchRaceData = async () => {
            setRanking(await getRankingForRace(raceId));
            setStartList(await getRemainingStartListForRace(raceId));
            setSplitTimes(await getSplittimesForCurrentSkier(raceId));
            setCurrentSkier(await getCurrentSkierForRace(raceId));
        };

        loadRace();
        fetchRaceData();

        const interval = setInterval(async () => {
            await fetchRaceData();
        }, 15000);

        return () => clearInterval(interval);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <ActiveRacePanel>
            <Details>
                <DetailPanelWrapper>
                    {race && <RaceDetailPanel race={race} />}
                </DetailPanelWrapper>

                <CurrentSkierPanel currentSkier={currentSkier} />
            </Details>
            <Lists>
                <div>
                    {startList && <StartListView startList={startList} />}
                </div>

                <div>{ranking && <RankingView raceRanking={ranking} />}</div>

                <div>
                    {' '}
                    {splitTimes && (
                        <SplitTimeListView
                            currentSkier={currentSkier}
                            splitTimes={splitTimes}
                        />
                    )}
                </div>
            </Lists>
        </ActiveRacePanel>
    );
};
