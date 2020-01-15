import React from 'react';
import styled from 'styled-components';
import { StartListView } from '../shared/race/startlist-view/StartListView';
import { RankingListView } from '../shared/race/ranking-view/RankingListView';
import { getRankingForRace, getStartListForRace } from '../../common/api';
import { useAsync } from 'react-async-hook';
import { RaceRanking } from '../../models/RaceRanking';
import { StartList } from '../../models/StartList';
import { LoadingWrapper } from '../shared/LoadingWrapper';

const ListsGrid = styled.div`
    display: grid;
    grid-template-columns: 1fr 2fr;
    column-gap: ${props => props.theme.gap};
    height: 100%;
`;

export const RaceListsPanel: React.FC<{
    raceId: number;
}> = ({ raceId }) => {
    const { loading, error, result } = useAsync(
        async (raceId: number): Promise<[RaceRanking[], StartList[]]> => {
            let ranking = await getRankingForRace(raceId);
            let startList = await getStartListForRace(raceId);

            return [ranking, startList];
        },
        [raceId]
    );

    return (
        <LoadingWrapper loading={loading} error={error}>
            <ListsGrid>
                {result?.[1] && <StartListView startList={result[1]} />}
                {result?.[0] && <RankingListView raceRanking={result[0]} />}
            </ListsGrid>
        </LoadingWrapper>
    );
};
