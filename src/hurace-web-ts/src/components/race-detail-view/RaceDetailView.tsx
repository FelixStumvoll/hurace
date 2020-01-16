import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from '../shared/race/RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { RaceWinnersView } from './winners-view/RaceWinnersView';
import { ColumnFlex, AlignRight, RowFlex } from '../../theme/CustomComponents';

const RaceDetails = styled(RowFlex)`
    margin-bottom: ${props => props.theme.gap};
`;

const DetailPanelWrapper = styled.div`
    height: fit-content;
    margin-right: ${props => props.theme.gap};
`;

export const RaceDetailView: React.FC<{ raceId: number; seasonId: number }> = ({
    raceId,
    seasonId
}) => {
    return (
        <DetailViewWrapper
            url={`/season/${seasonId}`}
            backText=" Zurück zur Saison"
        >
            <ColumnFlex>
                <RaceDetails>
                    <DetailPanelWrapper>
                        <RaceDetailPanel raceId={raceId} />
                    </DetailPanelWrapper>

                    <AlignRight>
                        <RaceWinnersView raceId={raceId} />
                    </AlignRight>
                </RaceDetails>

                <RaceListsPanel raceId={raceId} />
            </ColumnFlex>
        </DetailViewWrapper>
    );
};
