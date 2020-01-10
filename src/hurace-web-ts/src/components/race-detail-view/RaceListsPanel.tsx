import React from 'react';
import styled from 'styled-components';
import { StartListView } from '../shared/race/startlist-view/StartListView';
import { useStateAsync } from '../../hooks/useStateAsync';
import { RankingListView } from '../shared/race/ranking-view/RankingListView';
import { getRankingForRace, getStartListForRace } from '../../common/api';

const ListsGrid = styled.div`
    display: grid;
    grid-template-columns: 1fr 2fr;
    column-gap: ${props => props.theme.gap};
    height: 100%;
`;

export const RaceListsPanel: React.FC<{
    raceId: number;
}> = ({ raceId }) => {
    const [raceRanking] = useStateAsync(getRankingForRace, raceId);
    const [startList] = useStateAsync(getStartListForRace, raceId);
    return (
        <ListsGrid>
            {startList && <StartListView startList={startList} />}
            {raceRanking && <RankingListView raceRanking={raceRanking} />}
        </ListsGrid>
    );
};
