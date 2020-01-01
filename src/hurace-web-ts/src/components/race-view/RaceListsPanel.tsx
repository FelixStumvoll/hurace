import React from 'react';
import styled from 'styled-components';
import { StartListView } from './list-views/startlist-view/StartListView';
import { RankingView } from './list-views/ranking-view/RankingView';

const GridPanel = styled.div`
    display: grid;
    grid-template-columns: 1fr 2fr;
    column-gap: 10px;
    height: 100%;
    overflow: hidden;
`;

export const RaceListsPanel: React.FC<{ raceId: number }> = ({ raceId }) => {
    return (
        <GridPanel>
            <StartListView raceId={raceId} />
            <RankingView raceId={raceId} />
        </GridPanel>
    );
};
