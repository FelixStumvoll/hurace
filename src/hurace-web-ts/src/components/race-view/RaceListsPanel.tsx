import React from 'react';
import styled from 'styled-components';
import { StartListView } from './list-views/startlist-view/StartListView';
import { RankingView } from './list-views/ranking-view/RankingView';
import { useStateAsync } from '../../hooks/useStateAsync';
import { getRankingForRace, getStartListForRace } from '../../common/api';

const GridPanel = styled.div`
    display: grid;
    grid-template-columns: 1fr 2fr;
    column-gap: 10px;
    height: 100%;
    overflow: hidden;
`;

export const RaceListsPanel: React.FC<{
    raceId: number;
}> = ({ raceId }) => {
    const [raceRanking] = useStateAsync(getRankingForRace, raceId);
    const [startList] = useStateAsync(getStartListForRace, raceId);
    return (
        <GridPanel>
            {startList && <StartListView startList={startList} />}
            {raceRanking && <RankingView raceRanking={raceRanking} />}
        </GridPanel>
    );
};
