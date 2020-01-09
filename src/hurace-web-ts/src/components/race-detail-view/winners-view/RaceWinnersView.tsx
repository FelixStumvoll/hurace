import React from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { SkierListViewItem } from '../../skier-list-view/SkierListViewItem';
import { RaceRanking } from '../../../models/RaceRanking';
import { RowFlex } from '../../../theme/CustomComponents';

const WinnerWrapper = styled.div`
    margin-left: ${props => props.theme.gap};

    :first-child {
        margin-left: 0;
    }
`;

export const RaceWinnersView: React.FC<{ winners: RaceRanking[] }> = ({
    winners
}) => {
    return (
        <HeaderCard headerText="Sieger">
            <RowFlex>
                {winners?.map(w => (
                    <WinnerWrapper key={w.position}>
                        <SkierListViewItem skier={w.startList.skier} />
                    </WinnerWrapper>
                ))}
            </RowFlex>
        </HeaderCard>
    );
};
