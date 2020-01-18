import React from 'react';
import styled from 'styled-components';
import { RaceDetailPanel } from '../shared/race/RaceDetailPanel';
import { RaceListsPanel } from './RaceListsPanel';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { RaceWinnersView } from './winners-view/RaceWinnersView';
import { ColumnFlex, AlignRight, RowFlex } from '../../theme/CustomComponents';

const RaceDetails = styled(RowFlex)`
    margin-bottom: ${props => props.theme.gap};

    @media screen and (max-width: ${props => props.theme.tabletSize}) {
        flex-direction: column;
    }
`;

const DetailPanelWrapper = styled.div`
    height: fit-content;
    margin-right: ${props => props.theme.gap};

    @media screen and (max-width: ${props => props.theme.tabletSize}) {
        margin-right: 0;
    }
`;

const WinnersWrapper = styled(AlignRight)`
    @media screen and (max-width: ${props => props.theme.tabletSize}) {
        margin-left: 0;
        margin-top: ${props => props.theme.gap};
    }
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

                    <WinnersWrapper>
                        <RaceWinnersView raceId={raceId} />
                    </WinnersWrapper>
                </RaceDetails>

                <RaceListsPanel raceId={raceId} />
            </ColumnFlex>
        </DetailViewWrapper>
    );
};
