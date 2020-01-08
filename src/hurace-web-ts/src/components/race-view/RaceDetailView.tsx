import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from './RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { useStateAsync } from '../../hooks/useStateAsync';
import { getRaceDetails } from '../../common/api';
import { GridAreaProps } from '../../interfaces/GridAreaProps';
import { RaceWinnersView } from './winners-view/RaceWinnersView';

const RacePanel = styled.div`
    display: grid;
    max-height: 100%;
    gap: 10px;
    grid-template-columns: 1fr auto;
    grid-template-rows: auto 1fr;
    grid-template-areas: 'detail winners' 'lists lists';
    overflow: hidden;
`;

const GridAreaWrapper = styled.div<GridAreaProps>`
    grid-area: ${props => props.gridArea};
`;

export const RaceDetailView: React.FC<{ raceId: number; seasonId: number }> = ({
    raceId,
    seasonId
}) => {
    const [race] = useStateAsync(getRaceDetails, raceId);

    return (
        <DetailViewWrapper
            url={`/season/${seasonId}`}
            backText=" Zurück zur Saison"
        >
            <RacePanel>
                <GridAreaWrapper gridArea="detail">
                    {race && <RaceDetailPanel race={race} />}
                </GridAreaWrapper>

                <GridAreaWrapper gridArea="winners">
                    <RaceWinnersView raceId={raceId} />
                </GridAreaWrapper>

                <GridAreaWrapper gridArea="lists">
                    <RaceListsPanel raceId={raceId} />
                </GridAreaWrapper>
            </RacePanel>
        </DetailViewWrapper>
    );
};
