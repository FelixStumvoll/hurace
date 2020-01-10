import React from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { SkierListViewItem } from '../../skier-list-view/SkierListViewItem';
import { RaceRanking } from '../../../models/RaceRanking';
import { RowFlex, TextBold } from '../../../theme/CustomComponents';

const WinnerWrapper = styled.div`
    margin-left: ${props => props.theme.gap};
    display: flex;
    flex-direction: column;
    :first-child {
        margin-left: 0;
    }
`;

const WinnerPosition = styled(TextBold)`
    margin-bottom: 10px;
`;

export const RaceWinnersView: React.FC<{ winners: RaceRanking[] }> = ({
    winners
}) => {
    return (
        <HeaderCard headerText="Sieger">
            <RowFlex>
                {winners?.map(w => (
                    <WinnerWrapper key={w.position}>
                        <WinnerPosition>{w.position}</WinnerPosition>
                        <SkierListViewItem skier={w.skier} />
                    </WinnerWrapper>
                ))}
            </RowFlex>
        </HeaderCard>
    );
};
