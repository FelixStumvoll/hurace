import React from 'react';
import { HeaderCard } from '../../shared/HeaderCard';
import styled from 'styled-components';
import { SkierListViewItem } from '../../skier-list-view/SkierListViewItem';
import { RowFlex, TextBold } from '../../../theme/CustomComponents';
import { useAsync } from 'react-async-hook';
import { getWinnersForRace } from '../../../common/api';
import { LoadingWrapper } from '../../shared/LoadingWrapper';

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

export const RaceWinnersView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const { loading, error, result: winners } = useAsync(getWinnersForRace, [
        raceId
    ]);

    return (
        <HeaderCard headerText="Sieger">
            <LoadingWrapper loading={loading} error={error}>
                <RowFlex>
                    {winners?.map(w => (
                        <WinnerWrapper key={w.position}>
                            <WinnerPosition>{w.position}</WinnerPosition>
                            <SkierListViewItem skier={w.skier} />
                        </WinnerWrapper>
                    ))}
                </RowFlex>
            </LoadingWrapper>
        </HeaderCard>
    );
};
